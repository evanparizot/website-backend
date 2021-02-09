import {Construct, Stack, StackProps, SecretValue, RemovalPolicy} from 'monocdk';
import { Artifact} from 'monocdk/aws-codepipeline';
import { CodeBuildAction, CodeBuildActionType, GitHubSourceAction } from 'monocdk/aws-codepipeline-actions';
import { BuildSpec, LinuxBuildImage, PipelineProject } from 'monocdk/lib/aws-codebuild';
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
                    'npm run build'
                ],
                synthCommand: 'npx cdk synth',
                subdirectory: 'cdk'
            })
        });

        const unitTests = new PipelineProject(this, 'UnitTest', {
            projectName: 'WebsiteUnitTests',
            environment: {
                buildImage: LinuxBuildImage.AMAZON_LINUX_2_3
            },
            buildSpec: BuildSpec.fromSourceFilename('config/unit-test.yml')
        });

        const compileCode = new PipelineProject(this, 'CompileCode', {
            projectName: 'Compile',
            environment: {
                buildImage: LinuxBuildImage.AMAZON_LINUX_2_3
            },
            buildSpec: BuildSpec.fromSourceFilename('config/compile.yml')
        });

        const compileStage = pipeline.addStage('Compile');
        compileStage.addActions(
            new CodeBuildAction({
                actionName: 'UnitTest',
                input: sourceArtifact,
                outputs: [
                    new Artifact()
                ],
                project: unitTests,
                type: CodeBuildActionType.TEST,
                runOrder: 1
            }),
            new CodeBuildAction({
                actionName: 'CompileCode',
                input: sourceArtifact,
                outputs: [
                    new Artifact()
                ],
                project: compileCode,
                type: CodeBuildActionType.BUILD,
                runOrder: 1
            })
        );

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