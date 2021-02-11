package com.evanparizot.projects.handler;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.amazonaws.services.lambda.runtime.events.APIGatewayProxyRequestEvent;
import com.amazonaws.services.lambda.runtime.events.APIGatewayProxyResponseEvent;
import com.evanparizot.projects.business.IProjectsManager;
import com.evanparizot.projects.business.ProjectsManager;
import com.evanparizot.projects.model.Project;
import com.evanparizot.projects.model.ProjectMetadata;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.inject.Inject;

public class ProjectsHandler implements RequestHandler<APIGatewayProxyRequestEvent, APIGatewayProxyResponseEvent> {

    private static final Logger logger = LoggerFactory.getLogger(ProjectsHandler.class);
    private final IProjectsManager manager;

    public ProjectsHandler() {
        this.manager = new ProjectsManager();
    }

    @Inject
    public ProjectsHandler(IProjectsManager manager) {
        this.manager = manager;
    }

    public APIGatewayProxyResponseEvent handleRequest(APIGatewayProxyRequestEvent event, Context context) {
        APIGatewayProxyResponseEvent response = new APIGatewayProxyResponseEvent();
        try {
            ObjectMapper mapper = new ObjectMapper();
            Project project = mapper.readValue(event.getBody(), Project.class);

            logger.debug(project.toString());

            response.setStatusCode(200);
            response.setBody("Did something");

        } catch (Exception e) {
            response.setStatusCode(500);
            response.setBody(e.getMessage());
        } finally {
            logger.debug("Completed doing stuff");
        }

        return response;
    }

    public APIGatewayProxyResponseEvent getProject(APIGatewayProxyRequestEvent event, Context context) {
        APIGatewayProxyResponseEvent response = new APIGatewayProxyResponseEvent();
        try {
            String something = manager.getProject("");
            System.out.println(something);
            Project project = Project.builder()
                    .metadata(ProjectMetadata.builder()
                            .title(manager.getProject(""))
                            .build()
                    ).build();
            ObjectMapper mapper = new ObjectMapper();

            response.setStatusCode(200);
            response.setBody(mapper.writeValueAsString(project));
        } catch (Exception e) {

        }
        return response;
    }
}
