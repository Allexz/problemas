
namespace ChainOfResponsibility;

public interface IAmmountHandler
{
    IAmmountHandler SetNext(IAmmountHandler handler);
    string Handle(double amount);
}

public abstract class AmmountHandler : IAmmountHandler
{
    protected IAmmountHandler _nextHandler;
    public abstract double TotalAmmount { get; }
    public abstract string Employee { get; }
    public abstract string OrderedBy(double  amount);

    public IAmmountHandler SetNext(IAmmountHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }
    public virtual string Handle(double amount)
    {
        if (amount >= TotalAmmount) return _nextHandler?.Handle(amount) ?? FallBackHandler(amount);

        return OrderedBy(amount);

    }

    private string? FallBackHandler(double amount) => $"No competency for this amount {amount:C2}";
}

public class ManagerHandler : AmmountHandler
{
    public override string Employee => "Manager";
    public override double TotalAmmount => 100000D;
    public override string OrderedBy(double amount) => $"This total amount ({amount}) will be operated by {Employee}";
}
public class DirectorHandler : AmmountHandler
{
    public override string Employee => "Director";
    public override double TotalAmmount => 2000000D;
    public override string OrderedBy(double amount) => $"This total amount ({amount}) will be operated by {Employee}";

}
public class SupervisorHandler : AmmountHandler
{
    public override string Employee => "Supervisor";
    public override double TotalAmmount => 30000D;
    public override string OrderedBy(double amount) => $"This total amount ({amount}) will be operated by {Employee}";
}

public class HandlerBuilder
{
    private IAmmountHandler _first;
    private IAmmountHandler _current;

    public HandlerBuilder AddHandler<T>() where T : IAmmountHandler, new()
    {
        var handler = new T();

        if (_first == null)
        {
            _first = handler;
            _current = handler;
        }
        else 
            _current = _current.SetNext(handler);
        return this;
    }

    public IAmmountHandler Build() => _first;
}

class Program
{
    static void Main(string[] args)
    {
        // Construindo a cadeia de handlers
        var handler = new HandlerBuilder()
            .AddHandler<ManagerHandler>()
            .AddHandler<DirectorHandler>()
            .AddHandler<SupervisorHandler>()
            .Build();

        while (true)
        {
            Console.WriteLine("Digite o valor a ser gerenciado (ou 'sair' para terminar):");
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

