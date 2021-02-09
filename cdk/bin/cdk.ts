import { App, Environment } from 'monocdk';
import { ROOT } from '../env/accounts';
import { WebsitePipelineStack } from '../lib/pipeline';


const app = new App();
new WebsitePipelineStack(app, 'WebsitePipelineStack', {
    env: ROOT
});
app.synth();