using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.AppointmentDtos;

namespace AutoService.Business.Services.Abstract;

public interface IAppointmentService
{
    Task<List<ResultAppointmentDto>>
        GetAllAsync();

    Task<AppointmentDetailDto?>
        GetByIdAsync(
            int appointmentId);

    Task AddAsync(
        CreateAppointmentDto dto);

    Task UpdateAsync(
        UpdateAppointmentDto dto);

    Task DeleteAsync(
        int appointmentId);

    Task<int> ConvertToServiceRecordAsync(
    int appointmentId);
}
