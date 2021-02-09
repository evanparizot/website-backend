import { CfnOutput, Construct, RemovalPolicy, Stack } from 'monocdk';
import { Code, Function, Runtime } from 'monocdk/aws-lambda';
import { AttributeType, BillingMode, Table, TableEncryption } from 'monocdk/aws-dynamodb';
import { ARecord, PublicHostedZone, RecordTarget } from 'monocdk/aws-route53';
import { ApiGateway } from 'monocdk/aws-route53-targets';
import { Certificate, CertificateValidation } from 'monocdk/aws-certificatemanager';
import { EndpointType, JsonSchemaType, JsonSchemaVersion, LambdaIntegration, RestApi, SecurityPolicy } from 'monocdk/aws-apigateway';
import * as path from 'path';
import { WebsiteStageProps } from './website-stage';

export interface WebsiteStackProps extends WebsiteStageProps {}

export class WebsiteStack extends Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: Construct, id: string, props: WebsiteStackProps) {
    super(scope, id, props);
    //
    // **************************************
    // Lambda(s)

    const runTime: Runtime = Runtime.JAVA_11;
    const handlerPath: string = 'com.evanparizot.projects.handler.ProjectsHandler';
    const projectCode: Code = Code.fromAsset(path.resolve(__dirname, '../../src/target/website-backend-1.0-SNAPSHOT.jar'));

    const projectsLambda = new Function(this, 'ProjectsLambda', {
      runtime: runTime,
      handler: handlerPath,
      code: projectCode,
    });

    const projectLambda = new Function(this, 'ProjectLambda', {
      runtime: runTime,
      handler: handlerPath + '::getProject',
      code: projectCode
    });
    
    //
    // **************************************
    // Dynamo
    
    // The code that defines your stack goes here
    const projectsTable = new Table(this, 'projects', {
      tableName: 'projects',
      partitionKey: {
        name: 'id',
        type: AttributeType.STRING
      },
      sortKey: {
        name: 'version',
        type: AttributeType.STRING
      },
      billingMode: BillingMode.PAY_PER_REQUEST,
      encryption: TableEncryption.DEFAULT,
      removalPolicy: RemovalPolicy.DESTROY
    });
    
    projectsTable.grantReadWriteData(projectsLambda);
    
    //
    // **************************************
    // APIGateway
    
    const zone = new PublicHostedZone(this, 'HostedZone', {
      zoneName: props.apiUrl
    });

    const certificate = new Certificate(this, 'Certificate', {
      domainName: props.apiUrl,
      validation: CertificateValidation.fromDns(zone)
    });
    
    const restApi = new RestApi(this, 'RestApi', {
      domainName: {
        domainName: props.apiUrl,
        certificate: certificate,
        endpointType: EndpointType.REGIONAL,
        securityPolicy: SecurityPolicy.TLS_1_2
      },
    });
    
    const responseModel = restApi.addModel('SuccessResponse', {
      contentType: 'application/json',
      schema: {
        schema: JsonSchemaVersion.DRAFT4,
        type: JsonSchemaType.OBJECT
      }
    });

    restApi.root.addResource('projects').addMethod('GET', new LambdaIntegration(projectsLambda));
    // restApi.root.addResource('projects/{id}').addMethod('GET', new LambdaIntegration(projectLambda));

    new ARecord(this, 'ARecord', {
      recordName: props.apiUrl,
      zone: zone,
      target: RecordTarget.fromAlias(new ApiGateway(restApi))
    });
    
    //
    // **************************************
  }
}