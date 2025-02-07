

using Fluid;
namespace SSO_Backend.Models.Mailing;

public class EmailTemplate
{
    public string? From { get; set; }
    public List<string> To { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public ILiquidTemplate Subject { get; set; } = null!;
    public ILiquidTemplate Body { get; set; } = null!;
    public bool IsBodyHtml { get; set; }
    public bool? Priority { get; set; }
    public List<Attachment> Attachments { get; set; }
    public EmailTemplate()
    {
        To = [];
        Cc = [];
        Bcc = [];
        Attachments = [];
    }
}

public interface ILiquidTemplate
{
    ValueTask<string> RenderAsync(object model);
}

public class FluidTemplateAdapter : ILiquidTemplate
{
    private readonly IFluidTemplate _fluidTemplate;
    public FluidTemplateAdapter(IFluidTemplate fluidTemplate)
    {
        _fluidTemplate = fluidTemplate;
    }
    public ValueTask<string> RenderAsync(object model)
    {

        var options = new TemplateOptions();
        options.MemberAccessStrategy.Register(model.GetType());
        var context = new TemplateContext(model, options);
        return _fluidTemplate.RenderAsync(context);
    }
}