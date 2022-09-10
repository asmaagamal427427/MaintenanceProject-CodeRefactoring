const string LARGE_FILTER_SIZE = "100";
const string SMALL_FILTER_SIZE = "10"; 

public int toNumber(String Input) {
    int n = 0;
    try {
        n = Convert.ToInt32(Input);
        if (n != 0) return n;
    } catch (Exception ex){ 
        Console.WriteLine("Input is not an integer"); // Give the user constructive feedback first
        return 0;
    }
}

public class OrderManager {

    private readonly IOrderStore orderStore;
    private string filterMode;   // mode variable defines whether the filter is a small order filter or large order filter 
    FilterAndWrite filter;

    public OrderManager(IOrderStore orderStore, filterMode) {
        this.orderStore = orderStore;
        var orders = orderStore.GetOrders();
        assert(filterMode == "large" || filterMode == "small");
        OrderFilter orderFilter = new OrderFilter(new OrderWriter(), orders, filterMode);
        filter = new FilterAndWrite(orderFilter);
    }

    public void WriteOutOrders() {
        filter.WriteOutFiltrdAndPriceSortedOrders(new OrderWriter());
    }

}

public class FilterAndWrite {

    OrderFilter orderFilter;
    string filterSize;
    private List<Order> orders;

    public FilterAndWrite(OrderFilter orderFilter){
        this.orderFilter = orderFilter;
        this.orders = orderFilter.orders;
        this.filterSize = orderFilter.filterSize;
    }

    public FilterAndWrite(List<Order> orders){
        this.orders = orders;
    }

    protected List<Order> FilterOrdersSmallerThan(List<Order> allOrders, string size){
        List<Order> filtered = new List<Order>();
        for (int i = 0; i <= allOrders.Count; i++){
            int number = toNumber(size);
            if (allOrders[i].Size <= number){
                continue;
            } else {
                filtered.Add(orders[i]);
            }
        }
        return filtered;
    }

    public void WriteOutFiltrdAndPriceSortedOrders(IOrderWriter writer) {
        List<Order> filteredOrders = this.FilterOrdersSmallerThan(orders, filterSize);
        Enumerable.OrderBy(filteredOrders, o => o.Price);
        ObservableCollection<Order> observableCollection = new ObservableCollection<Order>();
        foreach (Order o in filteredOrders){
            observableCollection.Add(o);
        }
        writer.WriteOrders(observableCollection);
    }
}

public class OrderFilter {

    private IOrderWriter orderWriter;
    private List<Order> orders;
    protected string filterSize;
    protected string mode = "large"; // Default value is large

    public OrderFilter(IOrderWriter orderWriter, List<Order> orders, string mode){
        this.orderWriter = orderWriter;
        this.orders = orders;
        this.mode = mode;
        if (mode == "large"){
            filterSize = LARGE_FILTER_SIZE;
        } else {
            filterSize = SMALL_FILTER_SIZE;
        }
    }
}

public class Order {

    private double dPrice;
    private int iSize;
    private string sSymbol;

    public double Price {
        get { return this.dPrice; }
        set { this.dPrice = value; }
    }
    public int Size {
        get { return this.iSize; }
        set { this.iSize = value; }
    }
    public string Symbol {
        get { return this.sSymbol; }
        set { this.sSymbol = value; }
    }
    
}

// These are stub interfaces that already exist in the system
// They're out of scope of the code review

public interface IOrderWriter {
    void WriteOrders(IEnumerable<Order> orders);
}

public class OrderWriter : IOrderWriter {
    public void WriteOrders(IEnumerable<Order> orders) {} // Just a stub
}

public interface IOrderStore {
    List<Order> GetOrders();
}

public class OrderStore : IOrderStore {
    public List<Order> GetOrders() {
        return new List<Order> { 
            new Order {
                Price = 10,
                Size =1,
                Symbol = "TShirt"
                }, 
            new Order {
                Price = 15,
                Size =2,
                Symbol = "Sport Goods"
            } 
        };
    }
}