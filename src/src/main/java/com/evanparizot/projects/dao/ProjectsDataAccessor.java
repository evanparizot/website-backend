package com.evanparizot.projects.dao;

import com.amazonaws.services.dynamodbv2.AmazonDynamoDB;

public class ProjectsDataAccessor implements IProjectsDataAccessor {

    private AmazonDynamoDB client;

    public ProjectsDataAccessor() {
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public String getProject(String id) {
        return null;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void saveProject() {
    }
}
