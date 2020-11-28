import { Construct, Stack, StackProps, SecretValue } from '@aws-cdk/core';
import { CdkPipeline, SimpleSynthAction } from '@aws-cdk/pipelines';
import * as codepipeline from '@aws-cdk/aws-codepipeline';
import * as codepipeline_actions from '@aws-cdk/aws-codepipeline-actions';
import { WebsiteStage } from './website-stage';
import * as codebuild from '@aws-cdk/aws-codebuild';
import { CodeBuildAction } from '@aws-cdk/aws-codepipeline-actions';

//https://aws.amazon.com/blogs/developer/cdk-pipelines-continuous-delivery-for-aws-cdk-applications/
//https://docs.aws.amazon.com/cdk/api/latest/docs/pipelines-readme.html

export class WebsitePipelineStack extends Stack {
    constructor(scope: Construct, id: string, props?: StackProps) {
        super(scope, id, props);

        // const githubSource = codebuild.Source.gitHub({
        //     owner: 'evanparizot',
        //     repo: 'website-backend',
        //     webhook: true,
        //     webhookFilters: [
        //         codebuild.FilterGroup
        //             .inEventOf(codebuild.EventAction.PULL_REQUEST_CREATED, codebuild.EventAction.PULL_REQUEST_UPDATED)
        //     ]
        // })

        const sourceArtifact = new codepipeline.Artifact();
        const cloudAssemblyArtifact = new codepipeline.Artifact();

        const pipeline = new CdkPipeline(this, 'Pipeline', {
            pipelineName: 'WebsitePipeline',
            cloudAssemblyArtifact,

            sourceAction: new codepipeline_actions.GitHubSourceAction({
                actionName: 'Github',
                output: sourceArtifact,
                oauthToken: SecretValue.secretsManager('github-token'),
                owner: 'evanparizot',
                repo: 'website-backend'
            }),

            synthAction: new SimpleSynthAction({
                sourceArtifact,
                cloudAssemblyArtifact,
                installCommands: [
                    'npm install',
                    'npm install -g aws-cdk'
                ],
                buildCommands: [
                    'mvn -f ./src/pom.xml package'
                ],
                synthCommand: 'npx cdk synth'
            })
        });

        // Define application stages here
        const betaStage = pipeline.addApplicationStage(new WebsiteStage(this, 'BetaStage', {
            env: { 
                account: '602915839163', 
                region: 'us-east-2'
            },
            apiUrl: 'api.beta.evanparizot.com'
        }));

        const prodStage = pipeline.addApplicationStage(new WebsiteStage(this, 'ProdStage', {
            env: {
                account: '893753507904',
                region: 'us-east-2'
            },
            apiUrl: 'api.evanparizot.com'
        }));
    }
}