package com.evanparizot.projects.dao;

public interface IProjectsDataAccessor {
    String getProject(String id);

    void saveProject();
}
