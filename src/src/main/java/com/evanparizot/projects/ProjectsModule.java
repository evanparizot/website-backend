package com.evanparizot.projects;

import com.evanparizot.projects.business.IProjectsManager;
import com.evanparizot.projects.business.ProjectsManager;
import dagger.Module;
import dagger.Provides;

@Module
public class ProjectsModule {

    @Provides
    public static IProjectsManager provideProjectsManager() {
        return new ProjectsManager();
    }
}
