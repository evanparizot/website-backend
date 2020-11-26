package com.evanparizot.projects;


import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.LambdaLogger;
import com.amazonaws.services.lambda.runtime.RequestHandler;

import java.util.Map;

public class ProjectsHandler implements RequestHandler<Map<String, String>, String> {
    public String handleRequest(Map<String, String> stringStringMap, Context context) {
        LambdaLogger logger = context.getLogger();
        logger.log("Hello there");
        return null;
    }
}
