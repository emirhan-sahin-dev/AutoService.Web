using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.CommonDtos;

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages =>
        TotalCount == 0
            ? 1
            : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage =>
        CurrentPage > 1;

    public bool HasNextPage =>
        CurrentPage < TotalPages;
}