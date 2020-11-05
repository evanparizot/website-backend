import { CfnOutput, Construct, Stage, StageProps } from '@aws-cdk/core';
import { WebsiteStack } from './website-stack';

export interface WebsiteStageProps extends StageProps {
    readonly apiUrl: string
}

export class WebsiteStage extends Stage {
    
    public readonly urlOutput: CfnOutput;

    constructor(scope: Construct, id: string, props: WebsiteStageProps) {
        super(scope, id, props);

        const service = new WebsiteStack(this, 'Website', props);

        this.urlOutput = service.urlOutput;
    }
}