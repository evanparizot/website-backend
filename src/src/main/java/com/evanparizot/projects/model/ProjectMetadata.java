package com.evanparizot.projects.model;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.*;

import java.util.Date;

@Getter
@Setter
@Builder
@EqualsAndHashCode
@NoArgsConstructor
@AllArgsConstructor
@JsonInclude(JsonInclude.Include.NON_NULL)
public class ProjectMetadata {
    private String title;
    private Date startDate;
    private Date completionDate;
    private String description;
    private String thumbnailUrl;
    private Boolean isDraft;
}
