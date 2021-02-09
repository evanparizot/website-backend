package com.evanparizot.projects.business;

import com.evanparizot.projects.ProjectsModule;
import dagger.Component;

@Component(modules = ProjectsModule.class)
public interface IProjectsManager {
    String getProject(String id);
}
