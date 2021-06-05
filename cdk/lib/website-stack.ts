import { CfnOutput, Construct, Duration, RemovalPolicy, Stack } from 'monocdk';
import { Code, Function, Runtime, Tracing } from 'monocdk/aws-lambda';
import { BlockPublicAccess, Bucket } from 'monocdk/aws-s3';
import { AttributeType, BillingMode, Table, TableEncryption } from 'monocdk/aws-dynamodb';
import { ARecord, HostedZone, RecordTarget } from 'monocdk/aws-route53';
import { ApiGateway } from 'monocdk/aws-route53-targets';
import { Certificate, CertificateValidation } from 'monocdk/aws-certificatemanager';
import { CorsOptions, EndpointType, LambdaIntegration, RestApi, SecurityPolicy } from 'monocdk/aws-apigateway';
import * as path from 'path';
import { WebsiteStageProps } from './website-stage';

export interface WebsiteStackProps extends WebsiteStageProps {
  hostedZoneId: string;
  defaultCorsPreflightOptions: CorsOptions
}

export class WebsiteStack extends Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: Construct, id: string, props: WebsiteStackProps) {
    super(scope, id, props);

    // **************************************
    // S3

    const projectsBucket = new Bucket(this, "ProjectsBucket", {
        blockPublicAccess: BlockPublicAccess.BLOCK_ALL,
        removalPolicy: RemovalPolicy.DESTROY,
    });

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
      tracing: Tracing.PASS_THROUGH,
      memorySize: 512,
      environment: {
        ASPNETCORE_ENVIRONMENT: props.environment,
        AwsResources__ProjectsBucketName: projectsBucket.bucketName
      },
      timeout: Duration.seconds(30)
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
    
    // **************************************
    // APIGateway
    
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
      defaultCorsPreflightOptions: props.defaultCorsPreflightOptions
    });

    restApi.addUsagePlan('UsagePlan', {
      name: 'Simple',
      throttle: {
        rateLimit: 5,
        burstLimit: 2
      }
    })
    
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
    // Permissions
    
    projectsBucket.grantReadWrite(projectsLambda);
    projectsTable.grantFullAccess(projectsLambda);

  }
}
