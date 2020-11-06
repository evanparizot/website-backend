import * as cdk from '@aws-cdk/core';
import * as s3 from '@aws-cdk/aws-s3';
import * as dynamodb from '@aws-cdk/aws-dynamodb';
import * as lambda from '@aws-cdk/aws-lambda';
import * as acm from '@aws-cdk/aws-certificatemanager';
import * as route53 from '@aws-cdk/aws-route53';
import * as alias from '@aws-cdk/aws-route53-targets';
import * as apigw from '@aws-cdk/aws-apigateway';
import { CfnOutput, Duration, StackProps } from '@aws-cdk/core';
import * as path from 'path';
import { WebsiteStageProps } from './website-stage';
import { ARecord, HostedZone, RecordTarget } from '@aws-cdk/aws-route53';
import { Certificate, CertificateValidation } from '@aws-cdk/aws-certificatemanager';
import { DomainName, EndpointType, LambdaIntegration, RestApi, SecurityPolicy } from '@aws-cdk/aws-apigateway';
import { domain } from 'process';

export interface WebsiteStackProps extends WebsiteStageProps {}

export class WebsiteStack extends cdk.Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: cdk.Construct, id: string, props: WebsiteStackProps) {
    super(scope, id, props);

    // **************************************
    // Dynamo

    // The code that defines your stack goes here
    const projectsTable = new dynamodb.Table(this, 'projects-table', {
      partitionKey: {
        name: 'id',
        type: dynamodb.AttributeType.STRING
      },
      billingMode: dynamodb.BillingMode.PAY_PER_REQUEST,
      encryption: dynamodb.TableEncryption.DEFAULT
    });

    //
    // **************************************
    // Lambda(s)

    const projectsLambda = new lambda.Function(this, 'ProjectsLambda', {
      runtime: lambda.Runtime.PYTHON_3_8,
      code: lambda.Code.fromAsset(path.resolve(__dirname, '../src')),
      handler: 'projects.handler'
    });

    const projectsLambdaIntegration = new LambdaIntegration(projectsLambda);

    // const projectsLambdaIntegration = new LambdaProxyIntegration({
    //   handler: projectsLambda
    // });

    //
    // **************************************
    // APIGateway

    const zone = HostedZone.fromHostedZoneAttributes(this, 'domainzone', {
      hostedZoneId: 'Z2YUIRANSY13TZ',
      zoneName: 'evanparizot.com'
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

    restApi.root.addResource('projects').addMethod('GET', projectsLambdaIntegration);

    new ARecord(this, 'ARecord', {
      recordName: props.apiUrl,
      zone: zone,
      target: RecordTarget.fromAlias(new alias.ApiGateway(restApi))
    });

    //
    // **************************************
  }
}
