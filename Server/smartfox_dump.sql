--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: account; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE account (
    id integer NOT NULL,
    name character varying,
    password character varying,
    ip_address character varying,
    first_login date,
    last_login date,
    last_actor_id integer
);


ALTER TABLE public.account OWNER TO smartfox;

--
-- Name: account_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE account_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.account_id_seq OWNER TO smartfox;

--
-- Name: account_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE account_id_seq OWNED BY account.id;


--
-- Name: actor; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE actor (
    id integer NOT NULL,
    account_id integer,
    shape_id integer,
    name character varying,
    persona_id integer
);


ALTER TABLE public.actor OWNER TO smartfox;

--
-- Name: actor_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE actor_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.actor_id_seq OWNER TO smartfox;

--
-- Name: actor_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE actor_id_seq OWNED BY actor.id;


--
-- Name: actor_scene; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE actor_scene (
    id integer NOT NULL,
    actor_id integer,
    scene_id integer,
    playlist_id integer,
    target_id integer,
    start_x real,
    start_y real,
    start_z real,
    start_rot real,
    start_rot_x real,
    start_rot_y real,
    start_rot_z real,
    start_rot_w real,
    persona_id integer,
    mood_id integer,
    actor_group_id integer,
    behavior_tree_id integer
);


ALTER TABLE public.actor_scene OWNER TO smartfox;

--
-- Name: actor_scene_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE actor_scene_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.actor_scene_id_seq OWNER TO smartfox;

--
-- Name: actor_scene_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE actor_scene_id_seq OWNED BY actor_scene.id;


--
-- Name: actor_scene_sequence; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE actor_scene_sequence (
    id integer NOT NULL,
    actor_scene_id integer,
    sequence_id integer
);


ALTER TABLE public.actor_scene_sequence OWNER TO smartfox;

--
-- Name: actor_scene_sequence_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE actor_scene_sequence_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.actor_scene_sequence_id_seq OWNER TO smartfox;

--
-- Name: actor_scene_sequence_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE actor_scene_sequence_id_seq OWNED BY actor_scene_sequence.id;


--
-- Name: actor_scene_weapon; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE actor_scene_weapon (
    id integer NOT NULL,
    actor_scene_id integer,
    weapon_actor_scene_id integer,
    weapon_id integer,
    node_name character varying,
    attach_node character varying,
    offset_x real,
    offset_y real,
    offset_z real,
    orientation_x real,
    orientation_y real,
    orientation_z real,
    orientation_w real
);


ALTER TABLE public.actor_scene_weapon OWNER TO smartfox;

--
-- Name: actor_scene_weapon_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE actor_scene_weapon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.actor_scene_weapon_id_seq OWNER TO smartfox;

--
-- Name: actor_scene_weapon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE actor_scene_weapon_id_seq OWNED BY actor_scene_weapon.id;


--
-- Name: behavior_tree; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE behavior_tree (
    id integer NOT NULL,
    name character varying
);


ALTER TABLE public.behavior_tree OWNER TO smartfox;

--
-- Name: behavior_tree_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE behavior_tree_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.behavior_tree_id_seq OWNER TO smartfox;

--
-- Name: behavior_tree_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE behavior_tree_id_seq OWNED BY behavior_tree.id;


--
-- Name: behavior_tree_node; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE behavior_tree_node (
    id integer NOT NULL,
    behavior_tree_id integer,
    parent_node_id integer,
    node_type integer,
    name character varying,
    condition character varying,
    rule character varying,
    node_order integer,
    chart_x integer,
    chart_y integer,
    chart_z integer
);


ALTER TABLE public.behavior_tree_node OWNER TO smartfox;

--
-- Name: behavior_tree_node_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE behavior_tree_node_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.behavior_tree_node_id_seq OWNER TO smartfox;

--
-- Name: behavior_tree_node_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE behavior_tree_node_id_seq OWNED BY behavior_tree_node.id;


--
-- Name: playlist; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE playlist (
    id integer NOT NULL,
    skeleton_id integer,
    name character varying
);


ALTER TABLE public.playlist OWNER TO smartfox;

--
-- Name: playlist_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE playlist_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.playlist_id_seq OWNER TO smartfox;

--
-- Name: playlist_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE playlist_id_seq OWNED BY playlist.id;


--
-- Name: playlist_sequence; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE playlist_sequence (
    id integer NOT NULL,
    playlist_id integer,
    sequence_id integer,
    sequence_order real,
    repeats integer,
    speed real
);


ALTER TABLE public.playlist_sequence OWNER TO smartfox;

--
-- Name: playlist_sequence_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE playlist_sequence_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.playlist_sequence_id_seq OWNER TO smartfox;

--
-- Name: playlist_sequence_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE playlist_sequence_id_seq OWNED BY playlist_sequence.id;


--
-- Name: scene; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE scene (
    id integer NOT NULL,
    mission_id integer,
    name character varying,
    bounds_x_min real,
    bounds_x_max real,
    bounds_y_min real,
    bounds_y_max real,
    bounds_z_min real,
    bounds_z_max real
);


ALTER TABLE public.scene OWNER TO smartfox;

--
-- Name: scene_event; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE scene_event (
    id integer NOT NULL,
    actor_id integer,
    scene_id integer,
    type integer,
    "time" real,
    duration real,
    node integer,
    value_x real,
    value_y real,
    value_z real,
    action character varying,
    sequence character varying,
    target_id integer,
    acting_group_id integer,
    target_group_id integer,
    frequency integer,
    time_range real,
    delay_type integer
);


ALTER TABLE public.scene_event OWNER TO smartfox;

--
-- Name: scene_event_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE scene_event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.scene_event_id_seq OWNER TO smartfox;

--
-- Name: scene_event_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE scene_event_id_seq OWNED BY scene_event.id;


--
-- Name: scene_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE scene_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.scene_id_seq OWNER TO smartfox;

--
-- Name: scene_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE scene_id_seq OWNED BY scene.id;


--
-- Name: sequence; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE sequence (
    id integer NOT NULL,
    skeleton_id integer,
    filename character varying,
    name character varying
);


ALTER TABLE public.sequence OWNER TO smartfox;

--
-- Name: sequence_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE sequence_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.sequence_id_seq OWNER TO smartfox;

--
-- Name: sequence_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE sequence_id_seq OWNED BY sequence.id;


--
-- Name: shape; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE shape (
    id integer NOT NULL,
    filename character varying
);


ALTER TABLE public.shape OWNER TO smartfox;

--
-- Name: shape_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE shape_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.shape_id_seq OWNER TO smartfox;

--
-- Name: shape_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE shape_id_seq OWNED BY shape.id;


--
-- Name: weapon; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE weapon (
    id integer NOT NULL,
    fxflexbody_id integer,
    name character varying
);


ALTER TABLE public.weapon OWNER TO smartfox;

--
-- Name: weapon_attack; Type: TABLE; Schema: public; Owner: smartfox; Tablespace: 
--

CREATE TABLE weapon_attack (
    id integer NOT NULL,
    weapon_id integer,
    sequence_id integer,
    name character varying,
    type integer,
    minrange real,
    maxrange real,
    force_x real,
    force_y real,
    force_z real,
    startframe integer,
    endframe integer,
    damage real,
    count integer
);


ALTER TABLE public.weapon_attack OWNER TO smartfox;

--
-- Name: weapon_attack_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE weapon_attack_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.weapon_attack_id_seq OWNER TO smartfox;

--
-- Name: weapon_attack_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE weapon_attack_id_seq OWNED BY weapon_attack.id;


--
-- Name: weapon_id_seq; Type: SEQUENCE; Schema: public; Owner: smartfox
--

CREATE SEQUENCE weapon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.weapon_id_seq OWNER TO smartfox;

--
-- Name: weapon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: smartfox
--

ALTER SEQUENCE weapon_id_seq OWNED BY weapon.id;


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY account ALTER COLUMN id SET DEFAULT nextval('account_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY actor ALTER COLUMN id SET DEFAULT nextval('actor_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY actor_scene ALTER COLUMN id SET DEFAULT nextval('actor_scene_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY actor_scene_sequence ALTER COLUMN id SET DEFAULT nextval('actor_scene_sequence_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY actor_scene_weapon ALTER COLUMN id SET DEFAULT nextval('actor_scene_weapon_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY behavior_tree ALTER COLUMN id SET DEFAULT nextval('behavior_tree_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY behavior_tree_node ALTER COLUMN id SET DEFAULT nextval('behavior_tree_node_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY playlist ALTER COLUMN id SET DEFAULT nextval('playlist_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY playlist_sequence ALTER COLUMN id SET DEFAULT nextval('playlist_sequence_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY scene ALTER COLUMN id SET DEFAULT nextval('scene_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY scene_event ALTER COLUMN id SET DEFAULT nextval('scene_event_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY sequence ALTER COLUMN id SET DEFAULT nextval('sequence_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY shape ALTER COLUMN id SET DEFAULT nextval('shape_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY weapon ALTER COLUMN id SET DEFAULT nextval('weapon_id_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: smartfox
--

ALTER TABLE ONLY weapon_attack ALTER COLUMN id SET DEFAULT nextval('weapon_attack_id_seq'::regclass);


--
-- Data for Name: account; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY account (id, name, password, ip_address, first_login, last_login, last_actor_id) FROM stdin;
1	chris	\N	192.168.1.12	\N	\N	\N
\.


--
-- Name: account_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('account_id_seq', 1, true);


--
-- Data for Name: actor; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY actor (id, account_id, shape_id, name, persona_id) FROM stdin;
1	1	1	player	\N
2	0	4	swat_1	\N
3	0	4	swat_2	\N
4	0	4	swat_3	\N
5	0	4	swat_4	\N
6	0	4	swat_5	\N
7	0	4	swat_6	\N
8	0	4	swat_7	\N
9	0	4	swat_8	\N
10	0	4	swat_9	\N
11	0	4	swat_10	\N
12	0	4	swat_11	\N
13	0	4	swat_12	\N
14	0	4	swat_13	\N
15	0	4	swat_14	\N
16	0	4	swat_15	\N
17	0	4	swat_16	\N
18	0	4	swat_17	\N
19	0	4	swat_18	\N
20	0	4	swat_19	\N
21	0	4	swat_20	\N
22	0	4	swat_21	\N
23	0	4	swat_22	\N
24	0	4	swat_23	\N
25	0	4	swat_24	\N
26	0	4	swat_25	\N
27	0	4	swat_26	\N
28	0	4	swat_27	\N
29	0	4	swat_28	\N
30	0	4	swat_29	\N
31	0	4	swat_30	\N
32	0	4	swat_31	\N
33	0	4	swat_32	\N
34	0	4	swat_33	\N
35	0	4	swat_34	\N
36	0	4	swat_35	\N
37	0	4	swat_36	\N
38	0	4	swat_37	\N
39	0	4	swat_38	\N
40	0	4	swat_39	\N
41	0	4	swat_40	\N
42	0	5	enemy_1	\N
43	0	5	enemy_2	\N
44	0	5	enemy_3	\N
45	0	5	enemy_4	\N
46	0	5	enemy_5	\N
47	0	5	enemy_6	\N
48	0	5	enemy_7	\N
49	0	5	enemy_8	\N
50	0	5	enemy_9	\N
51	0	5	enemy_10	\N
\.


--
-- Name: actor_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('actor_id_seq', 51, true);


--
-- Data for Name: actor_scene; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY actor_scene (id, actor_id, scene_id, playlist_id, target_id, start_x, start_y, start_z, start_rot, start_rot_x, start_rot_y, start_rot_z, start_rot_w, persona_id, mood_id, actor_group_id, behavior_tree_id) FROM stdin;
1	5	1	\N	\N	180	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
2	6	1	\N	\N	176	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
3	7	1	\N	\N	172	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
4	8	1	\N	\N	168	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
5	9	1	\N	\N	164	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
6	10	1	\N	\N	160	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
7	11	1	\N	\N	156	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
8	12	1	\N	\N	152	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
9	13	1	\N	\N	148	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
10	14	1	\N	\N	144	5	-150	\N	\N	\N	\N	\N	\N	\N	\N	1
11	15	1	\N	\N	180	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
12	16	1	\N	\N	176	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
13	17	1	\N	\N	172	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
14	18	1	\N	\N	168	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
15	19	1	\N	\N	164	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
16	20	1	\N	\N	160	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
17	21	1	\N	\N	156	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
18	22	1	\N	\N	152	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
19	23	1	\N	\N	148	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
20	24	1	\N	\N	144	5	-155	\N	\N	\N	\N	\N	\N	\N	\N	1
21	25	1	\N	\N	180	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
22	26	1	\N	\N	176	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
23	27	1	\N	\N	172	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
24	28	1	\N	\N	168	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
25	29	1	\N	\N	164	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
26	30	1	\N	\N	160	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
27	31	1	\N	\N	156	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
28	32	1	\N	\N	152	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
29	33	1	\N	\N	148	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
30	34	1	\N	\N	144	5	-160	\N	\N	\N	\N	\N	\N	\N	\N	1
31	35	1	\N	\N	180	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
32	36	1	\N	\N	176	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
33	37	1	\N	\N	172	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
34	38	1	\N	\N	168	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
35	39	1	\N	\N	164	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
36	40	1	\N	\N	160	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
37	41	1	\N	\N	156	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
38	42	1	\N	\N	152	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
39	43	1	\N	\N	148	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
40	44	1	\N	\N	144	5	-165	\N	\N	\N	\N	\N	\N	\N	\N	1
41	5	2	\N	\N	125	13	-60	\N	\N	\N	\N	\N	\N	\N	\N	1
42	6	2	\N	\N	128	13	-60	\N	\N	\N	\N	\N	\N	\N	\N	1
43	7	2	\N	\N	131	13	-60	\N	\N	\N	\N	\N	\N	\N	\N	1
44	8	2	\N	\N	134	13	-60	\N	\N	\N	\N	\N	\N	\N	\N	1
\.


--
-- Name: actor_scene_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('actor_scene_id_seq', 44, true);


--
-- Data for Name: actor_scene_sequence; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY actor_scene_sequence (id, actor_scene_id, sequence_id) FROM stdin;
\.


--
-- Name: actor_scene_sequence_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('actor_scene_sequence_id_seq', 1, false);


--
-- Data for Name: actor_scene_weapon; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY actor_scene_weapon (id, actor_scene_id, weapon_actor_scene_id, weapon_id, node_name, attach_node, offset_x, offset_y, offset_z, orientation_x, orientation_y, orientation_z, orientation_w) FROM stdin;
\.


--
-- Name: actor_scene_weapon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('actor_scene_weapon_id_seq', 1, false);


--
-- Data for Name: behavior_tree; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY behavior_tree (id, name) FROM stdin;
1	test
\.


--
-- Name: behavior_tree_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('behavior_tree_id_seq', 1, true);


--
-- Data for Name: behavior_tree_node; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY behavior_tree_node (id, behavior_tree_id, parent_node_id, node_type, name, condition, rule, node_order, chart_x, chart_y, chart_z) FROM stdin;
1	1	0	3	testOne			0	\N	\N	\N
2	1	1	1	testTwo		Debug.Log("We are executing code from the database!!!!");	0	\N	\N	\N
3	1	1	1	testThree			1	\N	\N	\N
\.


--
-- Name: behavior_tree_node_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('behavior_tree_node_id_seq', 3, true);


--
-- Data for Name: playlist; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY playlist (id, skeleton_id, name) FROM stdin;
\.


--
-- Name: playlist_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('playlist_id_seq', 1, false);


--
-- Data for Name: playlist_sequence; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY playlist_sequence (id, playlist_id, sequence_id, sequence_order, repeats, speed) FROM stdin;
\.


--
-- Name: playlist_sequence_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('playlist_sequence_id_seq', 1, false);


--
-- Data for Name: scene; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY scene (id, mission_id, name, bounds_x_min, bounds_x_max, bounds_y_min, bounds_y_max, bounds_z_min, bounds_z_max) FROM stdin;
1	\N	test_one	100	200	-100	100	-300	-120
2	\N	test_two	100	200	0	200	-70	-50
\.


--
-- Data for Name: scene_event; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY scene_event (id, actor_id, scene_id, type, "time", duration, node, value_x, value_y, value_z, action, sequence, target_id, acting_group_id, target_group_id, frequency, time_range, delay_type) FROM stdin;
\.


--
-- Name: scene_event_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('scene_event_id_seq', 1, false);


--
-- Name: scene_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('scene_id_seq', 2, true);


--
-- Data for Name: sequence; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY sequence (id, skeleton_id, filename, name) FROM stdin;
\.


--
-- Name: sequence_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('sequence_id_seq', 1, false);


--
-- Data for Name: shape; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY shape (id, filename) FROM stdin;
1	player
2	swatRagdoll
3	enemy
\.


--
-- Name: shape_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('shape_id_seq', 3, true);


--
-- Data for Name: weapon; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY weapon (id, fxflexbody_id, name) FROM stdin;
\.


--
-- Data for Name: weapon_attack; Type: TABLE DATA; Schema: public; Owner: smartfox
--

COPY weapon_attack (id, weapon_id, sequence_id, name, type, minrange, maxrange, force_x, force_y, force_z, startframe, endframe, damage, count) FROM stdin;
\.


--
-- Name: weapon_attack_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('weapon_attack_id_seq', 1, false);


--
-- Name: weapon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: smartfox
--

SELECT pg_catalog.setval('weapon_id_seq', 1, false);


--
-- Name: account_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY account
    ADD CONSTRAINT account_pkey PRIMARY KEY (id);


--
-- Name: actor_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY actor
    ADD CONSTRAINT actor_pkey PRIMARY KEY (id);


--
-- Name: actor_scene_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY actor_scene
    ADD CONSTRAINT actor_scene_pkey PRIMARY KEY (id);


--
-- Name: actor_scene_sequence_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY actor_scene_sequence
    ADD CONSTRAINT actor_scene_sequence_pkey PRIMARY KEY (id);


--
-- Name: actor_scene_weapon_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY actor_scene_weapon
    ADD CONSTRAINT actor_scene_weapon_pkey PRIMARY KEY (id);


--
-- Name: behavior_tree_node_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY behavior_tree_node
    ADD CONSTRAINT behavior_tree_node_pkey PRIMARY KEY (id);


--
-- Name: behavior_tree_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY behavior_tree
    ADD CONSTRAINT behavior_tree_pkey PRIMARY KEY (id);


--
-- Name: playlist_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY playlist
    ADD CONSTRAINT playlist_pkey PRIMARY KEY (id);


--
-- Name: playlist_sequence_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY playlist_sequence
    ADD CONSTRAINT playlist_sequence_pkey PRIMARY KEY (id);


--
-- Name: scene_event_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY scene_event
    ADD CONSTRAINT scene_event_pkey PRIMARY KEY (id);


--
-- Name: scene_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY scene
    ADD CONSTRAINT scene_pkey PRIMARY KEY (id);


--
-- Name: sequence_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY sequence
    ADD CONSTRAINT sequence_pkey PRIMARY KEY (id);


--
-- Name: shape_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY shape
    ADD CONSTRAINT shape_pkey PRIMARY KEY (id);


--
-- Name: weapon_attack_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY weapon_attack
    ADD CONSTRAINT weapon_attack_pkey PRIMARY KEY (id);


--
-- Name: weapon_pkey; Type: CONSTRAINT; Schema: public; Owner: smartfox; Tablespace: 
--

ALTER TABLE ONLY weapon
    ADD CONSTRAINT weapon_pkey PRIMARY KEY (id);


--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

