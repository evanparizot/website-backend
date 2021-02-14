import { CfnOutput, Construct, Stage, StageProps } from 'monocdk';
import { WebsiteStack } from './website-stack';

export interface WebsiteStageProps extends StageProps {
    readonly environment: string;
    readonly hostedZoneId: string;
    readonly apiUrl: string;
}

export class WebsiteStage extends Stage {
    
    public readonly urlOutput: CfnOutput;

    constructor(scope: Construct, id: string, props: WebsiteStageProps) {
        super(scope, id, props);

        const service = new WebsiteStack(this, 'Website', props);

        this.urlOutput = service.urlOutput;
    }
}