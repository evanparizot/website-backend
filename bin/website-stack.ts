#!/usr/bin/env node
import { App, Environment } from '@aws-cdk/core';
import { WebsitePipelineStack } from '../lib/pipeline-stack';

const app = new App();
new WebsitePipelineStack(app, 'WebsitePipelineStack', {
    env: { 
        account: '008118432184',
        region: 'us-east-2'
    }
});
app.synth();