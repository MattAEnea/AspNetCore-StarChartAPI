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
                celObj.Satellites = celObjList.Where(o => o.OrbitedObjectId == id).ToList();
            }
            return Ok(celObj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.Where(o => o.Name == name).ToList();
            if (celObjList.Count == 0)
            {
                return NotFound();
            }
            foreach (CelestialObject celObj in celObjList)
            {
                celObj.Satellites = celObjList.Where(o => o.OrbitedObjectId == celObj.Id).ToList();
            }
            return Ok(celObjList);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            if (celObjList.Count == 0)
            {
                return NotFound();
            }
            foreach (CelestialObject celObj in celObjList)
            {
                celObj.Satellites = celObjList.Where(o => o.OrbitedObjectId == celObj.Id).ToList();
            }

            return Ok(celObjList);
        }

    }
}
