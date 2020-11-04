#!/usr/bin/env node
import { App } from '@aws-cdk/core';
import { WebsitePipelineStack } from '../lib/pipeline-stack';

const app = new App();

new WebsitePipelineStack(app, 'WebsitePipelineStack', {
    env: { 
        account: '591024261921',
        region: 'us-east-2'
    }
});

app.synth();