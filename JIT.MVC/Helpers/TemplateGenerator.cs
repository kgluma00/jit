using AutoMapper;
using JIT.Business.Interfaces;
using JIT.Core.DTOs;
using JIT.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIT.MVC.Helpers
{
	public class TemplateGenerator
	{

		public static string GetHTMLString(ICollection<ProjectViewModel> list, string username)
		{
			double sum = 0;

			foreach (var element in list)
			{
				sum += element.WorkingHours;
			}

			var sb = new StringBuilder();
			sb.Append($@"
						<html>
							<head>
								
							</head>
							<body>
								<div class='header'><h1>{username} Working Hours</h1></div>
								<table align='center'>
									<tr>
										<th>Project Name</th>
										<th>Working Date</th>
										<th>Working Hours</th>
									</tr>");

			foreach (var element in list)
			{
				sb.AppendFormat(@"<tr>
									<td>{0}</td>
									<td>{1}</td>
									<td>{2}</td>
								  </tr>", element.ProjectName, element.WorkingDate.ToString("dd/MM/yyyy"), element.WorkingHours);
			}

			sb.Append($@"
								</table>
								<div class='row'>
									<h1>Total hours: {sum}</h1>
								</div>
							</body>
						</html>");

			return sb.ToString();
		}

		internal static string GetHTMLString()
		{
			throw new NotImplementedException();
		}
	}
}
