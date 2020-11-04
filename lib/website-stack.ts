import * as cdk from '@aws-cdk/core';
import * as s3 from '@aws-cdk/aws-s3';
import * as dynamodb from '@aws-cdk/aws-dynamodb';
import * as apigwv2 from '@aws-cdk/aws-apigatewayv2';
import * as lambda from '@aws-cdk/aws-lambda';
import { HttpApi, HttpMethod, LambdaProxyIntegration } from '@aws-cdk/aws-apigatewayv2';
import { CfnOutput, Duration } from '@aws-cdk/core';
import * as path from 'path';

export class WebsiteStack extends cdk.Stack {

  public readonly urlOutput: CfnOutput;

  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    // The code that defines your stack goes here
    // const projectsTable = new dynamodb.Table(this, 'projects', {
    //   partitionKey: {
    //     name: '',
    //     type: dynamodb.AttributeType.STRING
    //   },
    //   billingMode: dynamodb.BillingMode.PAY_PER_REQUEST,
    //   encryption: dynamodb.TableEncryption.DEFAULT
    // });

    // const projectsLambda = new lambda.Function(this, 'ProjectsLambda', {
    //   runtime: lambda.Runtime.PYTHON_3_8,
    //   code: lambda.Code.fromAsset(path.resolve(__dirname, '/../../src/projects.py')),
    //   handler: 'index.main'
    // });

    // const projectsLambdaIntegration = new LambdaProxyIntegration({
    //   handler: projectsLambda
    // });

    const httpApi = new HttpApi(this, 'HttpApi', {
      corsPreflight: {
        allowHeaders: ['Authorization'],
        allowMethods: [HttpMethod.GET],
        allowOrigins: ['']
      }
    });

    // httpApi.addRoutes({
    //   path: '/projects',
    //   methods: [ HttpMethod.GET ],
    //   integration: projectsLambdaIntegration
    // });

  }
}
