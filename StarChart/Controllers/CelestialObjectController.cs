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
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            List<CelestialObject> celObjNames = celObjList.Where(o => o.Name == name).ToList();
            if (celObjNames.Count == 0)
            {
                return NotFound();
            }
            foreach (CelestialObject celObj in celObjNames)
            {
                celObj.Satellites = celObjList.Where(o => o.OrbitedObjectId == celObj.Id).ToList();
            }
            return Ok(celObjNames);
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celObj)
        {
            _context.CelestialObjects.Add(celObj);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celObj.Id }, celObj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject newCelObj)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            CelestialObject celObj = celObjList.FirstOrDefault(o => o.Id == id);
            if (celObjList.Count == 0 || celObj == null)
            {
                return NotFound();
            }
            celObj.Name = newCelObj.Name;
            celObj.OrbitalPeriod = newCelObj.OrbitalPeriod;
            celObj.OrbitedObjectId = newCelObj.OrbitedObjectId;
            _context.CelestialObjects.Update(celObj);
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            CelestialObject celObj = celObjList.FirstOrDefault(o => o.Id == id);
            if (celObjList.Count == 0 || celObj == null)
            {
                return NotFound();
            }
            celObj.Name = name;
            _context.CelestialObjects.Update(celObj);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> celObjList = _context.CelestialObjects.ToList();
            List<CelestialObject> celObjToDelete = celObjList.Where(o => o.Id == id).ToList();
            celObjToDelete.AddRange(celObjList.Where(o => o.OrbitedObjectId == id).ToList());
            if (celObjToDelete.Count == 0)
            {
                return NotFound();
            }
            _context.CelestialObjects.RemoveRange(celObjToDelete);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
