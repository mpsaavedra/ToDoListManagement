using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Tasks;

public record TaskCreateRequest(string Title, string Description, DateTime DueDate, string UserName);
