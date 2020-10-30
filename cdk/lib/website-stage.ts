import { CfnOutput, Construct, Stage, StageProps } from '@aws-cdk/core';
import { WebsiteStack } from './website-stack';

export class WebsiteStage extends Stage {
    public readonly urlOutput: CfnOutput;

    constructor(scope: Construct, id: string, props?: StageProps) {
        super(scope, id, props);

        const service = new WebsiteStack(this, 'Website');

        this.urlOutput = service.urlOutput;
    }
}