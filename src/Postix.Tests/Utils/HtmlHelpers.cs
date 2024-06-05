using System.Net;
using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using Postix.Tests.Extensions;

namespace Postix.Tests.Utils;

public static class HtmlHelpers
{
    public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var document = await BrowsingContext.New()
            .OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument)document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            if (response.RequestMessage != null)
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);
            return;

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
    
    public static async Task DoLogin(HttpClient client, string username, string password)
    {
        var response = await client.GetAsync("/Identity/Account/Login");
        var content = await HtmlHelpers.GetDocumentAsync(response);

        var form = (IHtmlFormElement)content.QuerySelector("form[id='account']");
        var loginResponse = await client.SendAsync(form,
            (IHtmlButtonElement)content.QuerySelector("button[type='submit']"),
            new Dictionary<string, string>()
            {
                ["Input_Email"] = username,
                ["Input_Password"] = password
            });

        var loginResponseContent = await HtmlHelpers.GetDocumentAsync(loginResponse);
        
        
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.Equal("http://localhost/", loginResponseContent.BaseUri);
    }
}