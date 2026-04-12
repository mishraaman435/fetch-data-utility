CREATE SCHEMA IF NOT EXISTS gis_db_access_users;

-- SEQUENCES
CREATE SEQUENCE IF NOT EXISTS gis_db_access_users.candidate_id_seq1;
CREATE SEQUENCE IF NOT EXISTS gis_db_access_users.designation_master_id_seq;
CREATE SEQUENCE IF NOT EXISTS gis_db_access_users.querylogs_query_id_seq;
CREATE SEQUENCE IF NOT EXISTS gis_db_access_users.users_comm_user_id_seq;

-- TABLES

CREATE TABLE gis_db_access_users.candidate (
	id SERIAL PRIMARY KEY,
	batch VARCHAR(50),
	scheduled_date VARCHAR,
	scheduled_time VARCHAR(50),
	c_rank INT,
	roll_no VARCHAR(50),
	full_name VARCHAR(50),
	email VARCHAR(50),
	mobile_number BIGINT,
	sms_send INT
);

CREATE TABLE gis_db_access_users.candidate_old (
	id INT PRIMARY KEY,
	batch VARCHAR(50),
	scheduled_date DATE,
	scheduled_time VARCHAR(50),
	c_rank INT,
	roll_no VARCHAR(50),
	full_name VARCHAR(50),
	email VARCHAR(50),
	mobile_number BIGINT,
	sms_send INT
);

CREATE TABLE gis_db_access_users.designation_master (
	id SERIAL PRIMARY KEY,
	designation VARCHAR NOT NULL
);

CREATE TABLE gis_db_access_users.querylogs (
	query_id SERIAL PRIMARY KEY,
	user_id INT,
	query_val VARCHAR,
	execution_time TIMESTAMP
);

CREATE TABLE gis_db_access_users.users_comm (
	user_id SERIAL PRIMARY KEY,
	first_name VARCHAR(50),
	last_name VARCHAR(50),
	mobile_number BIGINT,
	contact_email VARCHAR(50),
	user_name VARCHAR(50) UNIQUE NOT NULL,
	password VARCHAR(50) NOT NULL,
	tokens VARCHAR(50),
	otp_value INT,
	is_active INT,
	last_access TIMESTAMP,
	create_date TIMESTAMP,
	roll INT,
	designation INT
);

-- FUNCTION

CREATE OR REPLACE FUNCTION gis_db_access_users.fn_get_general_multipurpose(
	querytype VARCHAR,
	param_array TEXT[]
)
RETURNS SETOF REFCURSOR
LANGUAGE plpgsql
AS $$
DECLARE 
	ref1 REFCURSOR; 
	out_put_result INTEGER;
BEGIN

IF (querytype = 'GetVarifirdToken') THEN
    OPEN ref1 FOR
        SELECT uc.user_id, uc.first_name, uc.last_name, uc.mobile_number, uc.contact_email, 
               uc.user_name, uc.tokens, uc.is_active, uc.roll, dm.designation 
        FROM gis_db_access_users.users_comm uc 
        JOIN gis_db_access_users.designation_master dm 
        ON uc.designation = dm.id 
        WHERE uc.tokens = param_array[1] 
        AND uc.last_access >= NOW() - INTERVAL '15 minutes' 
        AND uc.is_active = 1;

    UPDATE gis_db_access_users.users_comm
    SET last_access = NOW()
    WHERE tokens = param_array[1] AND is_active = 1;

    RETURN NEXT ref1;
END IF;

IF(querytype='GetUserId') THEN	
	OPEN ref1 FOR
	SELECT user_id, user_name 
	FROM gis_db_access_users.users_comm 
	WHERE user_name = param_array[1] 
	AND password = param_array[2] 
	AND is_active = 1;
    RETURN NEXT ref1;
END IF;

IF querytype = 'GetTransaction' THEN
    IF param_array[1] = '0' THEN
        OPEN ref1 FOR
        SELECT ql.query_id, uc.user_id,
               CONCAT(uc.first_name, ' ', uc.last_name) AS Name, 
               dm.designation, ql.query_val
        FROM gis_db_access_users.querylogs ql
        JOIN gis_db_access_users.users_comm uc ON ql.user_id = uc.user_id
        JOIN gis_db_access_users.designation_master dm ON uc.designation = dm.id
        ORDER BY ql.query_id DESC
        LIMIT 1000;
    ELSE
        OPEN ref1 FOR
        SELECT query_val
        FROM gis_db_access_users.querylogs 
        WHERE user_id = param_array[1]::INT
        ORDER BY query_id DESC
        LIMIT 1000;
    END IF;
    RETURN NEXT ref1;
END IF;

IF(querytype='UpdateUserToken') THEN
	OPEN ref1 FOR
	UPDATE gis_db_access_users.users_comm
	SET tokens = param_array[1], last_access = NOW()
	WHERE user_id = param_array[2]::INT 
	RETURNING tokens;
	RETURN NEXT ref1;
END IF;

IF (querytype = 'InsertUser') THEN
    IF NOT EXISTS (
        SELECT 1 FROM gis_db_access_users.users_comm 
        WHERE user_name = param_array[5]
    ) THEN
        INSERT INTO gis_db_access_users.users_comm
        (first_name, last_name, mobile_number, contact_email, user_name, password, is_active, create_date, roll, designation)
        VALUES 
        (param_array[1], param_array[2], param_array[3]::BIGINT, param_array[4], param_array[5], param_array[6], 1, NOW(), param_array[7]::INT, param_array[8]::INT)
        RETURNING user_id INTO out_put_result;

        OPEN ref1 FOR SELECT out_put_result;
    ELSE
        out_put_result := -2;
        OPEN ref1 FOR SELECT out_put_result;
    END IF;
    RETURN NEXT ref1;
END IF;

IF(querytype='GetListOfCandidate') THEN	
	OPEN ref1 FOR
	SELECT * FROM gis_db_access_users.candidate 
	WHERE sms_send = param_array[3]::INT
	AND c_rank BETWEEN param_array[1]::INT AND param_array[2]::INT
	ORDER BY id;
	RETURN NEXT ref1;
END IF;

IF(querytype='GetDesignation') THEN	
	OPEN ref1 FOR
	SELECT id, designation FROM gis_db_access_users.designation_master;
	RETURN NEXT ref1;
END IF;

IF(querytype='InsertQuery') THEN
	INSERT INTO gis_db_access_users.querylogs(user_id, query_val, execution_time)
	SELECT user_id, query_val, NOW()
	FROM json_to_recordset(param_array[1]::JSON) AS tt(
		user_id INT,
		query_val TEXT
	);

	OPEN ref1 FOR SELECT 1;
	RETURN NEXT ref1;
END IF;

END;
$$;
