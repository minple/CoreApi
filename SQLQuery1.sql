select u.name as 'username', c.name as 'Country', ci.name as 'City', d.name as 'District' from Address as a
			join Country c on a.countryId = c.id
			join City ci on a.cityId = ci.id
			join District d on a.districtId = d.id
			join Users u on a.id = u.address2 or a.id = u.address1
			where u.id = 155


select u.name as 'Username', dt.name as 'Address Type', a.street as 'street', d.name 'District', ci.name as 'City', c.name as 'Country'
	from Users as u, Address as a, Country as c, City as ci, District as d, AddressType as dt
	where   a.countryId = c.id and
			a.cityId = ci.id and
			a.districtId = d.id and
			(a.id = u.address2 or
			a.id = u.address1) and
			a.addressTypeId = dt.id and
			u.id = 155
		  

