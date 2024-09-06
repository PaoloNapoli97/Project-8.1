using Microsoft.AspNetCore.Mvc;
using LabModel;
using LabServices;
using ComputerDTO;
using System.IO.Compression;


namespace LabApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class LabController : ControllerBase
    {
        LabService labService = new();

        [HttpGet]
        [Route("GetAllLabs")]
        public ActionResult<List<Lab>> GetLabs()
        {
            var labs = labService.ReadLabs();
            // foreach (var lab in labs) // Test check Deserialize value
            // {
            //     Console.WriteLine($"Lab {lab.Id}");
            //     foreach (var computer in lab.Computers)
            //     {
            //         Console.WriteLine($"Id {computer.Id}");
            //         Console.WriteLine($"Nome {computer.Name}");
            //         Console.WriteLine($"Descrizione {computer.Description}");
            //         Console.WriteLine($"Specifiche {computer.Specs}");
            //         Console.WriteLine($"Data {computer.Datetime}");
            //     }
            // }
            return Ok(labs);
        }

        [HttpPost]
        [Route("AddLab/{LabId}")]
        public ActionResult PostLab(string LabId)
        {
            var labs = labService.ReadLabs();
            var checkIfExist = labs.FirstOrDefault(x => x.Id == LabId);
            if (checkIfExist != null)
            {
                return BadRequest("Lab Id alredy exist");
            }
            else
            {
                Lab lab = new Lab(LabId);
                labs.Add(lab);
                var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                labService.WriteLabs(labs);
                return Created(pathToUrl, lab);
            }
        }

        [HttpPost]
        [Route("{LabId}/Add/Computer")]
        public ActionResult PostComputer(string LabId, [FromBody] ComputerAddDTO computerAddDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                if (computerAddDTO == null)
                {
                    return BadRequest();
                }
                else
                {
                    Computer computer = new Computer(computerAddDTO.Name, computerAddDTO.Description, computerAddDTO.Specs, computerAddDTO.Status, DateTime.Now);
                    lab.addComputer(computer);
                    var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                    labService.WriteLabs(labs);
                    return Created(pathToUrl, computer);
                }
            }
        }

        [HttpPost]
        [Route("{LabId}/Add/Resource")]
        public ActionResult AddResource(string LabId, [FromBody] ResourceDTO resourceDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest();
            }
            else
            {
                if (resourceDTO == null)
                {
                    return BadRequest();
                }
                else
                {
                    Resources resources = new Resources(resourceDTO.Name, resourceDTO.Description);
                    lab.addResource(resources);
                    var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                    labService.WriteLabs(labs);
                    return Created(pathToUrl, resources);
                }
            }
        }

        [HttpPatch]
        [Route("{LabId}/{Name}/Edit/Resources")]
        public ActionResult PatchComputer(string LabId, string Name, [FromBody] ResourceDTO resourceDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                var resource = lab.Resource.Find(x => x.Name == Name);
                if (resource == null)
                {
                    return BadRequest("Resource Name not found");
                }
                else
                {
                    if (resourceDTO.Name != null)
                    {
                        resource.Name = resourceDTO.Name;
                    }
                    if (resourceDTO.Description != null)
                    {
                        resource.Description = resourceDTO.Description;
                    }

                    labService.WriteLabs(labs);
                    return Ok(resource);
                }
            }
        }

        [HttpDelete]
        [Route("{LabId}/{Name}/Delete")]
        public ActionResult DeleteResource(string LabId, string Name)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return NotFound();
            }
            else
            {
                var resource = lab.Resource.Find(x => x.Name == Name);
                if (resource == null)
                {
                    return NotFound();
                }
                else
                {
                    lab.RemoveResource(resource);
                    labService.WriteLabs(labs);
                    return Ok(resource);
                }
            }
        }

        [HttpPut]
        [Route("{LabId}/{ComputerId}/MoveLab/{NewLabId}")]
        public ActionResult MoveComputer(string LabId, string NewLabId, string ComputerId)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                var computer = lab.Computers.Find(x => x.Id == ComputerId);
                if (computer == null)
                {
                    return BadRequest("Computerd Id doesn't exist");
                }
                else
                {
                    lab.RemoveComputer(computer);
                    var newLab = labs.FirstOrDefault(x => x.Id == NewLabId);
                    if (newLab == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        newLab.addComputer(computer);
                        var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                        labService.WriteLabs(labs);
                        return Created(pathToUrl, computer);
                    }
                }
            }
        }

        [HttpPost]
        [Route("{LabId}/{Id}/Software/Add")]
        public ActionResult PostSoftware(string LabId, string Id, [FromBody] SoftwareAddDTO softwareAddDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                var computer = lab.Computers.Find(x => x.Id == Id);
                if (computer == null)
                {
                    return BadRequest("Computerd Id doesn't exist");
                }
                else
                {
                    Software software = new Software(softwareAddDTO.Name);
                    computer.addSoftware(software);
                    var pathToUrl = Request.Path.ToString() + '/' + lab.Id;
                    labService.WriteLabs(labs);
                    return Created(pathToUrl, computer);
                }

            }
        }

        [HttpPatch]
        [Route("{LabId}/{Id}/Edit/Computer")]
        public ActionResult PatchComputer(string LabId, string Id, [FromBody] ComputerEditDTO computerEditDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                var computer = lab.Computers.Find(x => x.Id == Id);
                if (computer == null)
                {
                    return BadRequest("Computerd Id not found");
                }
                else
                {
                    if (computerEditDTO.Name != null)
                    {
                        computer.Name = computerEditDTO.Name;
                    }
                    if (computerEditDTO.Description != null)
                    {
                        computer.Description = computerEditDTO.Description;
                    }
                    if (computerEditDTO.Specs != null)
                    {
                        computer.Specs = computerEditDTO.Specs;
                    }
                    labService.WriteLabs(labs);
                    return Ok(computer);
                }
            }
        }

        [HttpPatch]
        [Route("{LabId}/{Id}/ChangeStatus")]
        public ActionResult ChangeStatus(string LabId, string Id, [FromBody] ComptuterStatusDTO comptuterStatusDTO)
        {
            var labs = labService.ReadLabs();
            var lab = labs.FirstOrDefault(x => x.Id == LabId);
            if (lab == null)
            {
                return BadRequest("Lab Id doen't exist");
            }
            else
            {
                var computer = lab.Computers.Find(x => x.Id == Id);
                if (computer == null)
                {
                    return BadRequest("Computerd Id not found");
                }
                else
                {
                    computer.Status = comptuterStatusDTO.StatusList;
                    labService.WriteLabs(labs);
                    return Ok(computer);
                }
            }
        }
    }
}
