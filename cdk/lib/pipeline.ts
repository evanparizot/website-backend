import {Construct, Stack, StackProps, SecretValue, RemovalPolicy} from 'monocdk';
import { Artifact} from 'monocdk/aws-codepipeline';
import { GitHubSourceAction } from 'monocdk/aws-codepipeline-actions';
import { CdkPipeline, SimpleSynthAction} from 'monocdk/pipelines';
import { BETA, PROD } from '../env/accounts';
import { WebsiteStage } from './website-stage';
//https://aws.amazon.com/blogs/developer/cdk-pipelines-continuous-delivery-for-aws-cdk-applications/
//https://docs.aws.amazon.com/cdk/api/latest/docs/pipelines-readme.html

export class WebsitePipelineStack extends Stack {
    constructor(scope: Construct, id: string, props?: StackProps) {
        super(scope, id, props);

        const sourceArtifact = new Artifact();
        const cloudAssemblyArtifact = new Artifact();

        const pipeline = new CdkPipeline(this, 'Pipeline', {
            pipelineName: 'WebsitePipeline',
            cloudAssemblyArtifact,

            sourceAction: new GitHubSourceAction({
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
                synthCommand: 'npx cdk synth',
                subdirectory: 'cdk'
            })
        });

        const betaStage = pipeline.addApplicationStage(new WebsiteStage(this, 'Beta', {
            env: BETA,
            apiUrl: 'api.beta.evanparizot.com'
        }));

        const prodStage = pipeline.addApplicationStage(new WebsiteStage(this, 'Prod', {
            env: PROD,
            apiUrl: 'api.evanparizot.com'
        }));
    }
}