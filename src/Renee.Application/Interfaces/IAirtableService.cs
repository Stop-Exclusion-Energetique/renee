using AirtableApiClient;
using Renee.Domain.ReneeError;
using System.Text.Json;

namespace Renee.Application.Interfaces;

public interface IAirtableService
{
	Task<ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>> AddRecordAsync(
			string? userName, 
			string? userEmail, 
			string? userReportingStructure, 
			string accompanyingFileReference, 
			Guid userId,
			double askedStopFound
		);
}
