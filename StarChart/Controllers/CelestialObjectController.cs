using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            CelestialObject celObj = celObjList.FirstOrDefault(o => o.Id == id);
            if(celObjList.Count == 0 || celObj == null)
            {
                return NotFound();
            }
            foreach (CelestialObject co in celObjList)
            {
                if(co.OrbitedObjectId == id) celObj.Satellites.Add(co);
            }
            return Ok(celObj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            CelestialObject celObj = celObjList.FirstOrDefault(o => o.Name == name);
            if (celObjList.Count == 0 || celObj == null)
            {
                return NotFound();
            }
            foreach (CelestialObject co in celObjList)
            {
                if (co.OrbitedObjectId == celObj.Id) celObj.Satellites.Add(co);
            }
            return Ok(celObj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            if (celObjList.Count == 0)
            {
                return NotFound();
            }
            for (int i = 0; i < celObjList.Count; i++)
            {
                for(int j = 0; j < celObjList.Count; i++)
                {
                    if (celObjList[i] != null && celObjList[j] != null)
                    {
                        if (celObjList[i].Id == celObjList[j].OrbitedObjectId)
                        {
                            celObjList[i].Satellites.Add(celObjList[j]);
                        }
                    }
                }    
            }
            return Ok(celObjList);
        }

    }
}
