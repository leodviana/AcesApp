using AcesApp.Interfaces;
using AcesApp.Models;
using AcesApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AcesApp.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response> getUsuario(Usuario usuario)
        {

            try
            {
                

                var jsonRequest = JsonConvert.SerializeObject(usuario);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());
               
                var url = "api/teste/Login";

                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Usuário ou Senha Incorretos",

                    };

                }

                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Usuario>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "login Ok",
                    Result = user
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
                throw;
            }
        }


        public async Task<Response> getEventos(Events evento)
        {

            try
            {


                var jsonRequest = JsonConvert.SerializeObject(evento);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());

                var url = "api/Aulas/GetEvents";

                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Problemas com o horario do contrato" + evento.contrato,

                    };

                }

                var result = await response.Content.ReadAsStringAsync();
                
                var Eventos = JsonConvert.DeserializeObject<List<Events>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = Eventos
                };

            }
            catch (Exception ex)
            {
               // return null;
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
               
            }
        }



        public async Task<Response> getRanking(Ranking ranking)
        {

            try
            {


                var jsonRequest = JsonConvert.SerializeObject(ranking);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());

                var url = "api/Ranking/GetRanking";

                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Problemas com a conexao do servidor!"

                    };

                }

                var result = await response.Content.ReadAsStringAsync();

                var Eventos = JsonConvert.DeserializeObject<List<Ranking>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = Eventos
                };

            }
            catch (Exception ex)
            {
                // return null;
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }


        

        public async Task<Response> getEventosfree(Events evento)
        {

            try
            {


                var jsonRequest = JsonConvert.SerializeObject(evento);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());

                var url = "api/Aulas/GetEventsFree";

                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Problemas com o horario do contrato" + evento.contrato,

                    };

                }

                var result = await response.Content.ReadAsStringAsync();
                var Eventos = JsonConvert.DeserializeObject<List<Events>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = Eventos
                };

            }
            catch (Exception ex)
            {
                // return null;
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }



       public async Task<Response> getprofessores(Events evento)
        {

            try
            {

                var jsonRequest = JsonConvert.SerializeObject(evento);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());

                var url = "api/Aulas/Getprofessores";

                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Problemas com os dados dos professores" ,

                    };

                }

                var result = await response.Content.ReadAsStringAsync();
                var Eventos = JsonConvert.DeserializeObject<List<Professor>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = Eventos
                };

            }
            catch (Exception ex)
            {
                // return null;
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }


        public async Task<Response> saveHorarios(long id , Events evento)
        {

            try
            {

                var jsonRequest = JsonConvert.SerializeObject(evento);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(App.Current.Resources["UrlAPI"].ToString());

                var url = "api/Aulas/SaveEvents?id=" +id;

                var response = await client.PostAsync(url, httpContent);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message =  result,

                    };

                }

                
               
                var Eventos = JsonConvert.DeserializeObject<Events>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = Eventos
                };

            }
            catch (Exception ex)
            {
                // return null;
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }
    }
}
