package com.evanparizot.projects.handler;

import com.amazonaws.services.dynamodbv2.AmazonDynamoDB;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClientBuilder;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapperConfig;
import com.amazonaws.services.dynamodbv2.document.DynamoDB;
import com.amazonaws.services.dynamodbv2.document.Table;
import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.LambdaLogger;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.amazonaws.services.lambda.runtime.events.APIGatewayProxyRequestEvent;
import com.amazonaws.services.lambda.runtime.events.APIGatewayProxyResponseEvent;
import com.evanparizot.projects.dao.model.Project;

public class ProjectsHandler implements RequestHandler<APIGatewayProxyRequestEvent, APIGatewayProxyResponseEvent> {

//    private AmazonDynamoDB client;
//    private DynamoDB dynamoDB;
//    private DynamoDBMapper mapper;

    public ProjectsHandler() {
//        this.client = AmazonDynamoDBClientBuilder.standard().build();
//
//        DynamoDBMapperConfig config = DynamoDBMapperConfig.builder().build();
//        this.mapper = new DynamoDBMapper(client);
//
//        this.dynamoDB = new DynamoDB(client);
//
//        String tableName = System.getenv("TABLE_NAME");
    }


    public APIGatewayProxyResponseEvent handleRequest(APIGatewayProxyRequestEvent event, Context context) {

//        LambdaLogger logger = context.getLogger();
//        mapper.load(Project.class, "", "");
//
//        APIGatewayProxyResponseEvent response = new APIGatewayProxyResponseEvent();
//        logger.log("Hello there");
//        logger.log(String.valueOf(event));
        return null;
    }

    private void something() {
//        Table table = dynamoDB.getTable("Blah");
    }
}
