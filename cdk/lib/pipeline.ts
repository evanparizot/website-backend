import {Construct, Stack, StackProps, SecretValue, RemovalPolicy} from 'monocdk';
import { Artifact} from 'monocdk/aws-codepipeline';
import { CodeBuildAction, CodeBuildActionType, GitHubSourceAction } from 'monocdk/aws-codepipeline-actions';
import { CdkPipeline, SimpleSynthAction} from 'monocdk/pipelines';
import { Project, FilterGroup, EventAction, BuildSpec, Source, PipelineProject, LinuxBuildImage, GitHubSourceCredentials } from 'monocdk/aws-codebuild';
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
                    'npm install -g aws-cdk',
                    'dotnet tool install -g Amazon.Lambda.Tools'
                ],
                buildCommands: [
                    'npm run build',
                    'dotnet-lambda package --project-location ../src/WebsiteLambda'
                ],
                synthCommand: 'npx cdk synth',
                subdirectory: 'cdk'
            })
        });

        new GitHubSourceCredentials(this, 'GithubSourceCreds', {
            accessToken: SecretValue.secretsManager('github-token')
        });

        const pullRequestProject = new Project(this, 'PullRequestTests', {
            projectName: 'PullRequest-UnitTests',
            buildSpec: BuildSpec.fromSourceFilename('config/unit-test.yml'),
            source: Source.gitHub({
                owner: 'evanparizot',
                repo: 'website-backend',
                webhook: true,
                webhookFilters: [
                    FilterGroup.inEventOf(EventAction.PULL_REQUEST_CREATED)
                    .andBranchIsNot('master'),
                    FilterGroup.inEventOf(EventAction.PULL_REQUEST_UPDATED)
                    .andBranchIsNot('master')
                ]
            }),
            environment: {
                buildImage: LinuxBuildImage.AMAZON_LINUX_2_3
            }
        });

        const unitTests = new PipelineProject(this, 'UnitTest', {
            projectName: 'WebsiteUnitTests',
            environment: {
                buildImage: LinuxBuildImage.AMAZON_LINUX_2_3
            },
            buildSpec: BuildSpec.fromSourceFilename('config/unit-test.yml')
        });

        const validationStage = pipeline.addStage('Validation');
        validationStage.addActions(
            new CodeBuildAction({
                actionName: 'UnitTest',
                input: sourceArtifact,
                outputs: [
                    new Artifact()
                ],
                project: unitTests,
                type: CodeBuildActionType.TEST,
                runOrder: 1
            })
        );

        const betaStage = pipeline.addApplicationStage(new WebsiteStage(this, 'Beta', {
            env: BETA,
            environment: "Beta",
            apiUrl: 'api.beta.evanparizot.com',
            hostedZoneId: 'Z07753612ZF8O3ZB9IAV3'
        }));

        const prodStage = pipeline.addApplicationStage(new WebsiteStage(this, 'Prod', {
            env: PROD,
            environment: "Production",
            apiUrl: 'api.evanparizot.com',
            hostedZoneId: 'Z0775933ISRJ5NXQAFEW'
        }));
    }
}