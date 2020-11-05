import { Construct, Stack, StackProps, SecretValue } from '@aws-cdk/core';
import { CdkPipeline, SimpleSynthAction } from '@aws-cdk/pipelines';
import * as codepipeline from '@aws-cdk/aws-codepipeline';
import * as codepipeline_actions from '@aws-cdk/aws-codepipeline-actions';
import { WebsiteStage } from './website-stage';

//https://aws.amazon.com/blogs/developer/cdk-pipelines-continuous-delivery-for-aws-cdk-applications/

export class WebsitePipelineStack extends Stack {
    constructor(scope: Construct, id: string, props?: StackProps) {
        super(scope, id, props);

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

            synthAction: SimpleSynthAction.standardNpmSynth({
                sourceArtifact,
                cloudAssemblyArtifact,

                // buildCommand: 'npm run build'
            })
        });

        // Define application stages here
        pipeline.addApplicationStage(new WebsiteStage(this, 'Staging', {
            env: { 
                account: '591024261921', 
                region: 'us-east-2'
            }
        }));
    }
}