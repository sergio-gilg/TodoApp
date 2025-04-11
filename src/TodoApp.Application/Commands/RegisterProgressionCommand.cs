using MediatR;
using System;

namespace TodoApp.Application.Commands;

public class RegisterProgressionCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Percent { get; set; }
}