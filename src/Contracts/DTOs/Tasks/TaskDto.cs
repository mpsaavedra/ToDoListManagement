using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.DTOs.Tasks;

public record TaskDto(long Id, string Title, string Description, string StateType, DateTime? DueDate);
