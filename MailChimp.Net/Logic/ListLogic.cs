﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;

namespace MailChimp.Net.Logic
{
    internal class ListLogic : BaseLogic, IListLogic
    {
        public ListLogic(string apiKey) : base(apiKey)
        {
        }

        public async Task<IEnumerable<List>> GetAllAsync(ListRequest request = null)
        {
            using (var client = CreateMailClient("lists"))
            {
                var response = await client.GetAsync(request?.ToQueryString());
                await response.EnsureSuccessMailChimpAsync();


                var listResponse = await response.Content.ReadAsAsync<ListResponse>();
                return listResponse.Lists;
            }
        }

        public async Task<List> GetAsync(string id)
        {
            using (var client = CreateMailClient("lists/"))
            {
                var response = await client.GetAsync($"{id}");
                await response.EnsureSuccessMailChimpAsync();

                return await response.Content.ReadAsAsync<List>();
            }
        }

        public async Task DeleteAsync(string listId)
        {
            using (var client = CreateMailClient("lists/"))
            {
                var response = await client.DeleteAsync(listId);
                await response.EnsureSuccessMailChimpAsync();
            }
        }

        public async Task<List> AddOrUpdateAsync(List list)
        {

            using (var client = CreateMailClient("lists/"))
            {
                HttpResponseMessage response;
                if (string.IsNullOrWhiteSpace(list.Id))
                {
                    response = await client.PostAsJsonAsync("", list);
                }
                else
                {
                    response = await client.PatchAsJsonAsync(list.Id, list);
                }

                await response.EnsureSuccessMailChimpAsync();

                return await response.Content.ReadAsAsync<List>();
            }
        }

    }
}