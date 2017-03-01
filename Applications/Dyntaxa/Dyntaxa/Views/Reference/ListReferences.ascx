﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArtDatabanken.WebApplication.Dyntaxa.Data.Reference.ReferenceInfoViewModel>" %>
<% var grid = new WebGrid(source: Model.References, rowsPerPage: 20, canSort: false); %>
                           
    <%:grid.GetHtml(
        tableStyle: "reference-grid",
        headerStyle: "head",
        alternatingRowStyle: "alt",
        columns: grid.Columns(                
                grid.Column(columnName: "Name", header: Model.Labels.ColumnTitleName),
                grid.Column(columnName: "Year", header: Model.Labels.ColumnTitleYear),
                grid.Column(columnName: "Text", header: Model.Labels.ColumnTitleText),                            
                grid.Column(columnName: "Usage", header: Model.Labels.ColumnTitleUsage)                                                          
            )
        )    
    %>
