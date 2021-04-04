## Overview
This is the source code for the backend of [evanparizot.com](https://evanparizot.com). Frontend code can be found [here](https://github.com/evanparizot/website). This project isn't in use yet, but will serve as the BFF for evanparizot.com. This repository is a split into two different portions: infrastructure and code. 

#### Infrastructure
All components are hosted in AWS with a simple API Gateway backed by a lambda function. All infrastructure is maintained via IAC using AWS' CDK to build, test and deploy. All code can be found under the [cdk](/cdk) folder. Code is deployed through 
Codepipeline, whose structure is also maintained via the same cdk package.

#### Code
As mentioned, the website backend is an API Gateway connected to a Lambda function. The Lambda function is written in .NetCore 3.1, and uses the `Amazon.Lambda.AspNetCoreServer` nuget package to allow for code structure to resemble a fairly standard
.NetCore web application. Currently it has a simplistic 3 tier architecture as the data storage is the thing that is flux the most.
