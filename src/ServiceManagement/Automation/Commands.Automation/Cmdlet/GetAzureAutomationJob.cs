﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Security.Permissions;
using Microsoft.Azure.Commands.Automation.Model;
using Microsoft.Azure.Commands.Automation.Common;

namespace Microsoft.Azure.Commands.Automation.Cmdlet
{
    /// <summary>
    /// Gets a Job object for automation.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureAutomationJob", DefaultParameterSetName = AutomationCmdletParameterSets.ByAll)]
    [OutputType(typeof(Microsoft.Azure.Commands.Automation.Model.Job))]
    public class GetAzureAutomationJob : AzureAutomationBaseCmdlet
    {
        /// <summary> 
        /// Gets or sets the job id. 
        /// </summary> 
        [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByJobId, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The job id.")] 
        [Alias("JobId")] 
        public Guid Id { get; set; } 
 
         /// <summary> 
         /// Gets or sets the runbook name of the job. 
         /// </summary> 
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByRunbookName, Mandatory = true, HelpMessage = "The runbook name of the job.")]
         [Alias("Name")] 
         public string RunbookName { get; set; }

         /// <summary> 
         /// Gets or sets the status of a job. 
         /// </summary> 
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByRunbookName, Mandatory = false, HelpMessage = "The runbook name of the job.")]
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByAll, Mandatory = false, HelpMessage = "Filter jobs based on their status.")]
         [ValidateSet("Completed", "Failed", "Queued", "Starting", "Resuming", "Running", "Stopped", "Stopping", "Suspended", "Suspending", "Activating")]
         public string Status { get; set; } 

         /// <summary> 
         /// Gets or sets the start time filter. 
         /// </summary> 
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByRunbookName, Mandatory = false, HelpMessage = "Filter jobs so that job start time >= StartTime.")]
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByAll, Mandatory = false, HelpMessage = "Filter jobs so that job start time >= StartTime.")]
         public DateTimeOffset? StartTime { get; set; } 
 
         /// <summary> 
         /// Gets or sets the end time filter. 
         /// </summary> 
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByRunbookName, Mandatory = false, HelpMessage = "Filter jobs so that job end time <= EndTime.")]
         [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByAll, Mandatory = false, HelpMessage = "Filter jobs so that job end time <= EndTime.")]
         public DateTimeOffset? EndTime { get; set; } 

        /// <summary>
        /// Execute this cmdlet.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void AutomationExecuteCmdlet()
        {
            IEnumerable<Microsoft.Azure.Commands.Automation.Model.Job> jobs; 
 
            if (this.Id != null && !Guid.Empty.Equals(this.Id)) 
             { 
                 // ByJobId 
                 jobs = new List<Microsoft.Azure.Commands.Automation.Model.Job> { this.AutomationClient.GetJob(this.AutomationAccountName, this.Id) }; 
             } 
             else if (this.RunbookName != null) 
             { 
                 // ByRunbookName 
                 jobs = this.AutomationClient.ListJobsByRunbookName(this.AutomationAccountName, this.RunbookName, this.StartTime, this.EndTime, this.Status); 
             } 
             else 
             { 
                 // ByAll 
                 jobs = this.AutomationClient.ListJobs(this.AutomationAccountName, this.StartTime, this.EndTime, this.Status); 
             } 
 
             this.WriteObject(jobs, true); 
        }
    }
}
