import { CfnOutput, Construct, RemovalPolicy, Stack } from 'monocdk';
import { Code, Function, Runtime } from 'monocdk/aws-lambda';
import { AttributeType, BillingMode, Table, TableEncryption } from 'monocdk/aws-dynamodb';
import { ARecord, HostedZone, PublicHostedZone, RecordTarget } from 'monocdk/aws-route53';
import { ApiGateway } from 'monocdk/aws-route53-targets';
import { Certificate, CertificateValidation } from 'monocdk/aws-certificatemanager';
import { EndpointType, JsonSchemaType, JsonSchemaVersion, LambdaIntegration, RestApi, SecurityPolicy } from 'monocdk/aws-apigateway';
import * as path from 'path';
import { WebsiteStageProps } from './website-stage';

export interface WebsiteStackProps extends WebsiteStageProps {
  hostedZoneId: string;
}

export class WebsiteStack extends Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: Construct, id: string, props: WebsiteStackProps) {
    super(scope, id, props);
    // **************************************
    // Lambda(s)

    const handlerPath: string = 'WebsiteLambda::WebsiteLambda.LambdaEntryPoint::FunctionHandlerAsync';
    const projectCode: Code = Code.fromAsset(
      path.resolve(__dirname, '../../src/WebsiteLambda/bin/Release/netcoreapp3.1/WebsiteLambda.zip'));

    const projectsLambda = new Function(this, 'WebsiteLambda', {
      functionName: 'WebsiteLambda',
      runtime: Runtime.DOTNET_CORE_3_1,
      handler: handlerPath,
      code: projectCode,
    });

    // **************************************
    // Dynamo
    
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
    
    // **************************************
    // APIGateway
    
    // const zone = new PublicHostedZone(this, 'HostedZone', {
    //   zoneName: props.apiUrl
    // });

    const zone = HostedZone.fromHostedZoneAttributes(this, 'HostedZone', {
      zoneName: props.apiUrl,
      hostedZoneId: props.hostedZoneId
    });

    const certificate = new Certificate(this, 'Certificate', {
      domainName: props.apiUrl,
      validation: CertificateValidation.fromDns(zone)
    });

    const restApi = new RestApi(this, 'WebsiteApi', {
      restApiName: 'WebsiteApi',
      domainName: {
        domainName: props.apiUrl,
        certificate: certificate,
        endpointType: EndpointType.REGIONAL,
        securityPolicy: SecurityPolicy.TLS_1_2
      },
    });

    restApi.root.addProxy({
      defaultIntegration: new LambdaIntegration(projectsLambda),
      anyMethod: true
    });
    
    new ARecord(this, 'ARecord', {
      recordName: props.apiUrl,
      zone: zone,
      target: RecordTarget.fromAlias(new ApiGateway(restApi))
    });
    
    // **************************************
  }
}
