using AutoMapper;
using TareasMVC.Entidades;
using TareasMVC.Models;

namespace TareasMVC
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Tarea,TareaDTO>(); 

        }
    }
}
