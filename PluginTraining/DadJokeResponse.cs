using System.Runtime.Serialization;
[DataContract()]
public partial class DadJokeResponse
{

    [DataMember()]
    public int current_page;

    [DataMember()]
    public int limit;

    [DataMember()]
    public int next_page;

    [DataMember()]
    public int previous_page;

    [DataMember()]
    public Results[] results;

    [DataMember()]
    public string search_term;

    [DataMember()]
    public int status;

    [DataMember()]
    public int total_jokes;

    [DataMember()]
    public int total_pages;
}

// Type created for JSON at <<root>> --> results
[DataContract(Name = "results")]
public partial class Results
{

    [DataMember()]
    public string id;

    [DataMember()]
    public string joke;
}