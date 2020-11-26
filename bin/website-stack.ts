#!/usr/bin/env node
import { App, Environment } from '@aws-cdk/core';
import { WebsitePipelineStack } from '../lib/pipeline-stack';
import { WebsiteStack } from '../lib/website-stack';

const app = new App();

const staging: Environment = {
    account: '591024261921',
    region: 'us-east-2'
};

new WebsitePipelineStack(app, 'WebsitePipelineStack', {
    env: staging
});

new WebsiteStack(app, 'WebsiteStack', {
    apiUrl: ''
});

app.synth();