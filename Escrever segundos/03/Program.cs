using System;

namespace ChainOfResponsibilityTimeFormatting
{
    // Interface comum para todos os handlers
    public interface ITimeHandler
    {
        string Handle(long seconds);
        ITimeHandler SetNext(ITimeHandler next);
    }

    // Classe base abstrata para os handlers
    public abstract class TimeHandlerBase : ITimeHandler
    {
        private ITimeHandler _next;

        public virtual string Handle(long seconds)
         {
            if (seconds < UnitSeconds)
            {
                return _next?.Handle(seconds) ?? FormatFallback(seconds);
            }

            var value = seconds / UnitSeconds;
            var remaining = seconds % UnitSeconds;

            if (remaining > 0)
            {
                var nextResult = _next?.Handle(remaining);
                return $"{FormatUnit(value)} e {nextResult}";
            }

            return FormatUnit(value);
        }

        public ITimeHandler SetNext(ITimeHandler next)
        {
            _next = next;
            return next;
        }

        protected abstract string FormatUnit(long value);
        protected abstract long UnitSeconds { get; }
        protected virtual string FormatFallback(long seconds) => $"{seconds} segundo{(seconds != 1 ? "s" : "")}";
    }

    // Implementações concretas dos handlers
    public class YearHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 31536000; // 365 dias
        protected override string FormatUnit(long value) => $"{value} ano{(value > 1 ? "s" : "")}";
    }

    public class MonthHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 2592000; // 30 dias
        protected override string FormatUnit(long value) => $"{value} mês{(value > 1 ? "es" : "")}";
    }

    public class DayHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 86400; // 24 horas
        protected override string FormatUnit(long value) => $"{value} dia{(value > 1 ? "s" : "")}";
    }

    public class HourHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 3600; // 60 minutos
        protected override string FormatUnit(long value) => $"{value} hora{(value > 1 ? "s" : "")}";
    }

    public class MinuteHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 60;
        protected override string FormatUnit(long value) => $"{value} minuto{(value > 1 ? "s" : "")}";
    }

    public class SecondHandler : TimeHandlerBase
    {
        protected override long UnitSeconds => 1;
        protected override string FormatUnit(long value) => $"{value} segundo{(value > 1 ? "s" : "")}";
    }

    // Builder para construir a cadeia de responsabilidade
    public class HandlerBuilder
    {
        private ITimeHandler _first;
        private ITimeHandler _current;

        public HandlerBuilder AddHandler<T>() where T : ITimeHandler, new()
        {
            var handler = new T();

            if (_first == null)
            {
                _first = handler;
                _current = handler;
            }
            else
            {
                _current = _current.SetNext(handler);
            }

            return this;
        }

        public ITimeHandler Build()
        {
            return _first;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Construindo a cadeia de handlers
            var handler = new HandlerBuilder()
                .AddHandler<YearHandler>()
                .AddHandler<MonthHandler>()
                .AddHandler<DayHandler>()
                .AddHandler<HourHandler>()
                .AddHandler<MinuteHandler>()
                .AddHandler<SecondHandler>()
                .Build();

            while (true)
            {
                Console.WriteLine("Digite o número de segundos (ou 'sair' para terminar):");
                var input = Console.ReadLine();

                if (input?.ToLower() == "sair")
                    break;

                if (long.TryParse(input, out var seconds))
                {
                    var result = handler.Handle(seconds);
                    Console.WriteLine($"Resultado: {result}");
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número válido.");
                }
            }
        }
    }
}