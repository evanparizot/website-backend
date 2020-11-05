import * as cdk from '@aws-cdk/core';
import * as s3 from '@aws-cdk/aws-s3';
import * as dynamodb from '@aws-cdk/aws-dynamodb';
import * as apigwv2 from '@aws-cdk/aws-apigatewayv2';
import * as lambda from '@aws-cdk/aws-lambda';
import * as acm from '@aws-cdk/aws-certificatemanager';
import { DomainName, HttpApi, HttpMethod, LambdaProxyIntegration } from '@aws-cdk/aws-apigatewayv2';
import { CfnOutput, Duration, StackProps } from '@aws-cdk/core';
import * as path from 'path';
import { WebsiteStageProps } from './website-stage';
import { HostedZone } from '@aws-cdk/aws-route53';
import { Certificate, CertificateValidation } from '@aws-cdk/aws-certificatemanager';

export interface WebsiteStackProps extends WebsiteStageProps {}

export class WebsiteStack extends cdk.Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: cdk.Construct, id: string, props: WebsiteStackProps) {
    super(scope, id, props);

    // **************************************
    // Dynamo

    // The code that defines your stack goes here
    // const projectsTable = new dynamodb.Table(this, 'projects', {
    //   partitionKey: {
    //     name: '',
    //     type: dynamodb.AttributeType.STRING
    //   },
    //   billingMode: dynamodb.BillingMode.PAY_PER_REQUEST,
    //   encryption: dynamodb.TableEncryption.DEFAULT
    // });

    //
    // **************************************
    // Lambda(s)

    const projectsLambda = new lambda.Function(this, 'ProjectsLambda', {
      runtime: lambda.Runtime.PYTHON_3_8,
      code: lambda.Code.fromAsset(path.resolve(__dirname, 'lambda')),
      handler: 'projects.handler'
    });

    const projectsLambdaIntegration = new LambdaProxyIntegration({
      handler: projectsLambda
    });

    //
    // **************************************
    // APIGateway

    const zone = HostedZone.fromHostedZoneId(this, 'domainzone', 'Z2YUIRANSY13TZ');

    const certificate = new Certificate(this, 'Certificate', {
      domainName: props.apiUrl,
      validation: CertificateValidation.fromDns(zone)
    });

    const domain = new DomainName(this, 'HttpApiDomain', {
      domainName: props.apiUrl,
      certificate: certificate
    });

    const httpApi = new HttpApi(this, `${id}HttpApi`, {
      defaultDomainMapping: {
        domainName: domain
      },
      corsPreflight: {
        allowHeaders: ['Authorization'],
        allowMethods: [HttpMethod.GET],
        allowOrigins: ['*']
      }
    });

    httpApi.addRoutes({
      path: '/projects',
      methods: [ HttpMethod.GET ],
      integration: projectsLambdaIntegration
    });
    
    //
    // **************************************
  }
}
