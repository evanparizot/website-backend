import { CfnOutput, Construct, Stage, StageProps } from 'monocdk';
import { CorsOptions } from 'monocdk/aws-apigateway';
import { WebsiteStack } from './website-stack';

export interface WebsiteStageProps extends StageProps {
    readonly environment: string;
    readonly hostedZoneId: string;
    readonly apiUrl: string;
    readonly defaultCorsPreflightOptions: CorsOptions;
}

export class WebsiteStage extends Stage {
    
    public readonly urlOutput: CfnOutput;

    constructor(scope: Construct, id: string, props: WebsiteStageProps) {
        super(scope, id, props);

        const service = new WebsiteStack(this, 'Website', props);

        this.urlOutput = service.urlOutput;
    }
}