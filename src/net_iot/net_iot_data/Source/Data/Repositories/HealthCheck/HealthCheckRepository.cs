using Microsoft.EntityFrameworkCore;
using net_iot_data.Constants;
using net_iot_data.Data.Entities;
using net_iot_data.Data.Models;

namespace net_iot_data.Data.Repositories.HealthCheck
{
    public class HealthCheckRepository : IHealthCheckRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HealthCheckRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<HealthCheckDto> Create(HealthCheckDto command)
        {
            var dateTime = DateTime.Now;

            HealthCheckEntity newHealthCheck = new HealthCheckEntity()
            {
                Success = command!.Success,
                Action = CrudAction.Create,
                PageId = command.PageId,
                LastUpdatedBy = command.UserId,
                EstablishedBy = command.UserId,
                EstablishedDateTime = dateTime,
                LastUpdatedDateTime = dateTime
            };

            this._dbContext.HealthChecks.Add(newHealthCheck);

            await this._dbContext.SaveChangesAsync();

            return new HealthCheckDto()
            {
                HealthCheckKey = newHealthCheck.HealthCheckKey,
                Success = newHealthCheck.Success,
                Action = CrudAction.Create,
                PageId = command.PageId,
                LastUpdatedBy = command.UserId,
                EstablishedBy = command.UserId,
                EstablishedDateTime = dateTime,
                LastUpdatedDateTime = dateTime
            };
        }

        public async Task<HealthCheckDto> Delete(HealthCheckDto command)
        {
            HealthCheckEntity recordToDelete = new HealthCheckEntity()
            {
                HealthCheckKey = command!.HealthCheckKey
            };

            this._dbContext.HealthChecks.Remove(recordToDelete);

            await this._dbContext.SaveChangesAsync();

            return new HealthCheckDto()
            {
                HealthCheckKey = command.HealthCheckKey,
                Success = command.Success
            };
        }

        public async Task<HealthCheckDto?> Get(HealthCheckDto command)
        {
            var recordToReturn = await this._dbContext.HealthChecks
                                                .Where(hc => hc.HealthCheckKey == command!.HealthCheckKey)
                                                .Select(hc => new HealthCheckDto
                                                {
                                                    HealthCheckKey = hc.HealthCheckKey,
                                                    Success = hc.Success,
                                                    Action = hc.Action,
                                                    PageId = hc.PageId,
                                                    LastUpdatedBy = hc.LastUpdatedBy,
                                                    EstablishedBy = hc.EstablishedBy,
                                                    EstablishedDateTime = hc.EstablishedDateTime,
                                                    LastUpdatedDateTime = hc.LastUpdatedDateTime
                                                })
                                                .FirstOrDefaultAsync();

            if (recordToReturn != null)
            {
                return recordToReturn;
            }

            return null;
        }

        public async Task<List<HealthCheckDto>> GetAll()
        {
            var records = await (from hc in this._dbContext.HealthChecks
                        select new HealthCheckDto()
                        {
                            HealthCheckKey = hc.HealthCheckKey,
                            Success = hc.Success,
                            Action = hc.Action,
                            PageId = hc.PageId,
                            LastUpdatedBy = hc.LastUpdatedBy,
                            EstablishedBy = hc.EstablishedBy,
                            EstablishedDateTime = hc.EstablishedDateTime,
                            LastUpdatedDateTime = hc.LastUpdatedDateTime
                        }).ToListAsync();

            return records;
        }

        public async Task<HealthCheckDto?> Update(HealthCheckDto command)
        {
            var dateTime = DateTime.Now;
            var recordToUpdate = this._dbContext.HealthChecks.Where(hc => hc.HealthCheckKey == command!.HealthCheckKey).FirstOrDefault();

            if (recordToUpdate != null)
            {
                recordToUpdate.Success = command!.Success;
                recordToUpdate.Action = CrudAction.Update;
                recordToUpdate.PageId = command.PageId;
                recordToUpdate.LastUpdatedBy = command.UserId;
                recordToUpdate.LastUpdatedDateTime = dateTime;

                this._dbContext.HealthChecks.Update(recordToUpdate);

                await _dbContext.SaveChangesAsync();

                return new HealthCheckDto()
                {
                    HealthCheckKey = recordToUpdate.HealthCheckKey,
                    Success = recordToUpdate.Success,
                    Action = CrudAction.Update,
                    PageId = recordToUpdate.PageId,
                    LastUpdatedBy = recordToUpdate.LastUpdatedBy,
                    LastUpdatedDateTime = recordToUpdate.LastUpdatedDateTime,
                    EstablishedBy = recordToUpdate.EstablishedBy,
                    EstablishedDateTime = recordToUpdate.EstablishedDateTime
                };
            }

            return null;
        }
    }
}