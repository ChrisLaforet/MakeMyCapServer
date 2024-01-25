﻿namespace MakeMyCapAdmin.CQS.Query;

public class DistributorSkusQuery : IQuery
{
	public string DistributorCode { get; }

	public DistributorSkusQuery(string distributorCode)
	{
		DistributorCode = distributorCode;
	}
}